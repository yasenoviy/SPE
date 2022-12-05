using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Timers;
using Microsoft.Win32;

namespace Task_Queue
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //System.Diagnostics.Debugger.Launch();
            string Path = @"HKEY_LOCAL_MACHINE\SOFTWARE\Task_Queue\Parameters";
            System.Diagnostics.Debugger.Launch();
            Timer t = new Timer(int.Parse(Registry.GetValue(Path, "Task_Claim_Check_Period", RegistryValueKind.String).ToString()) * 1000);
            t.Elapsed += new ElapsedEventHandler(WorkingCycle);
            t.Enabled = true;
            WriteLog(" Service Task_Queue is STARTED");
            System.Threading.Thread TH = new System.Threading.Thread(StartProgressCycle);
            TH.Start();
            System.Threading.Thread TM = new System.Threading.Thread(ProgressingTasksCycle);
            TM.Start();
        }

        private static void StartProgressCycle()
        {
            string Path = "HKEY_LOCAL_MACHINE\\Software\\Task_Queue\\Parameters";
            Timer t2 = new Timer(int.Parse(Registry.GetValue(Path, "Task_Execution_Duration", RegistryValueKind.String).ToString()) * 1000);
            t2.Elapsed += new ElapsedEventHandler(WorkingCycleWhichStartsTasks);
            t2.Enabled = true;
        }

        private static void ProgressingTasksCycle()
        {
            Timer t2 = new Timer(2000);
            t2.Elapsed += new ElapsedEventHandler(WorkingCycleWhichProgressingTasks);
            t2.Enabled = true;
        }

        public static bool TaskValidation(string task)
        {
            string temp = "";
            string Path = "Software\\Task_Queue\\Tasks";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(Path);

            foreach (string name in key.GetValueNames())
                temp += name;

            if (task == "" || task.Substring(0, 5) != "Task_" || task.Substring(5).Length != 4 || int.Parse((task.Substring(5))).GetType().Name != "Int32" || temp.Contains(task))
            {
                return false;
            }
            return true;
        }

        protected override void OnStop()
        {
            WriteLog(" Service Task_Queue is STOPPED");
        }

        private static void WorkingCycle(object source, ElapsedEventArgs e)
        {
            string task = GetTask();

            if (TaskValidation(task))
            {
                AddNewTaskInTasks(task);
                DeleteTaskFromClaims(task);
            }
            else
            {
                if (task == "" || task == null)
                {
                    int lol = 1 + 1;
                }
                else
                {
                    WriteLog(" ПОМИЛКА розміщення заявки " + task);
                    DeleteTaskFromClaims(task);
                }
                
            }
        }

        private static void WorkingCycleWhichStartsTasks(object source, ElapsedEventArgs e)
        {
            string[] Task = GetTaskToDo();
            for (int i = 0; i < Task.Length; i++)
            {
                MakeTaskInProgress(Task[i]);
            }

        }

        private static void WorkingCycleWhichProgressingTasks(object source, ElapsedEventArgs e)
        {
            string Path = "Software\\Task_Queue\\Tasks";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(Path, true);
            string[] tasks = GetTasksToProgress();
            for (int i = 0; i < tasks.Length; i++)
            {
                int dotes = CountDotes(ref tasks[i]);
                string reCreatedTask = "";
                if (dotes != 1)
                {
                    string task = tasks[i].Substring(0, 21);
                    reCreatedTask = makeIs(16 - dotes + 1) + makeDotes(dotes - 1);
                    key.DeleteValue(tasks[i]);
                    key.SetValue(task, reCreatedTask, RegistryValueKind.String);//Task_0000-in progress
                }
                else
                {
                    reCreatedTask = makeIs(16 - dotes + 1) + makeDotes(dotes - 1);
                    string task = tasks[i].Substring(0, 9) + "-COMPLETED";
                    key.DeleteValue(tasks[i]);
                    key.SetValue(task, reCreatedTask, RegistryValueKind.String);
                    WriteLog(" " + tasks[i].Substring(0, 9) + " Успішно Виконана!!!");
                }
            }

        }

        private static string makeDotes(int numberOfDotes)
        {
            string dotes = "";
            for (int i = 0; i < numberOfDotes; i++)
            {
                dotes += "0";
            }
            return dotes;
        }

        private static string makeIs(int numberOfIs)
        {
            string Is = "";
            for (int i = 0; i < numberOfIs; i++)
            {
                Is += "8";
            }
            return Is;
        }

        private static int CountDotes(ref string task)
        {
            string dotes = "";
            int dotesLength = 0;
            string Path = "Software\\Task_Queue\\Tasks";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(Path);
            if (task != null && task != "") // TASK_XXXX-In progres
            {
                dotes = key.GetValue(task).ToString();
                dotes = dotes.Substring(0);
                for (int i = 0; i < dotes.Length; i++)
                {
                    if (dotes[i] == '0')
                    {
                        dotesLength++;
                    }
                }
            }
            return dotesLength;
        }

        private static string[] GetTasksToProgress()
        {
            int tasksLength = 0;
            string Path = "Software\\Task_Queue\\Tasks";

            RegistryKey key = Registry.LocalMachine.OpenSubKey(Path);

            foreach (string name in key.GetValueNames())
            {
                if (name.Contains("prog"))
                {
                    tasksLength++;
                }
            }

            string[] tasks = new string[tasksLength];

            tasksLength--;
            foreach (string name in key.GetValueNames())
                if (name.Contains("prog"))
                {
                    tasks[tasksLength] = name.ToString();
                    //tasksLength--;
                }
            return tasks;
        }

        private static string[] GetTaskToDo()
        {
            string Path = "HKEY_LOCAL_MACHINE\\Software\\Task_Queue\\Parameters";
            string[] task = new string[1];
            task[0] = "";
            int NumberOfTasksToProgress = int.Parse(Registry.GetValue(Path, "Task_Execution_Quantity", RegistryValueKind.String).ToString());
            if (HowManyTasksIsQueuedNow() >= NumberOfTasksToProgress)
            {
                NumberOfTasksToProgress -= HowManyTasksIsProgressingNow();
            }
            else if (HowManyTasksIsQueuedNow() <= NumberOfTasksToProgress - HowManyTasksIsProgressingNow())
            {
                NumberOfTasksToProgress = HowManyTasksIsQueuedNow();
            }

            if (NumberOfTasksToProgress > 0)
            {
                task = new string[NumberOfTasksToProgress];

                string Path2 = "Software\\Task_Queue\\Tasks";

                RegistryKey key = Registry.LocalMachine.OpenSubKey(Path2);

                foreach (string name in key.GetValueNames())
                {
                    if (name.Contains("Queued") && NumberOfTasksToProgress > 0)
                    {
                        task[NumberOfTasksToProgress - 1] = name.ToString();
                        NumberOfTasksToProgress--;
                    }
                }
            }
            return task;
        }

        private static int HowManyTasksIsQueuedNow()
        {

            int numberOfTasksIsQueued = 0;
            string temp = "";
            string Path = "Software\\Task_Queue\\Tasks";

            RegistryKey key = Registry.LocalMachine.OpenSubKey(Path);

            foreach (string name in key.GetValueNames())
                temp += name;


            if (temp.Contains("Queued"))
            {
                numberOfTasksIsQueued++;
            }

            return numberOfTasksIsQueued;
        }

        private static int HowManyTasksIsProgressingNow()
        {
            int numberOfTasksInProgress = 0;
            string Path = "Software\\Task_Queue\\Tasks";

            RegistryKey key = Registry.LocalMachine.OpenSubKey(Path);

            foreach (string name in key.GetValueNames())
            {
                if (name.Contains("progress"))
                {
                    numberOfTasksInProgress++;
                }
            }
            return numberOfTasksInProgress;
        }

        private static string GetTask()
        {
            string temp = "";
            string Path = "Software\\Task_Queue\\Claims";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(Path);

            try
            {
                foreach (string name in key.GetValueNames())
                    temp = name;
            }
            catch (Exception e)
            {
                return temp;
            }

            return temp;

        }

        public static void AddNewTaskInTasks(string task)
        {
            string Path = "Software\\Task_Queue\\Tasks";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(Path, true);
            string temp = task + "-Queued";
            string zero = "0000000000000000";
            key.SetValue(temp, zero/*, RegistryValueKind.String*/); // Task_0000-Queued
        }

        public static void MakeTaskInProgress(string task)
        {
            string Path = "Software\\Task_Queue\\Tasks";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(Path, true);
            if (task != "" && task != null)
            {
                key.DeleteValue(task);
                string temp = task.Substring(0, 9) + "-In progress";
                string zero = "0000000000000000";
                key.SetValue(temp, zero/*, RegistryValueKind.String*/);
            }
        }

        public static void DeleteTaskFromClaims(string task)
        {
            string Path = @"SOFTWARE\Task_Queue\Claims";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(Path, true);
            if (task != "")
                key.DeleteValue(task);
        }
        
        private static void WriteLog(string z)
        {
            using (StreamWriter F = new StreamWriter("C:\\Windows\\Logs\\TaskQueue.log", true))
            {
                F.WriteLine(DateTime.Now + z);
            }
        }
    }
}