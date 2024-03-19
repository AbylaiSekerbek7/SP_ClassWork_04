using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace ClassWork_04
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Random rnd = new Random((int)DateTime.Now.Ticks);

        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Выберите программу для запуска";
            ofd.Filter = "Исполняемые файлы (*.exe)|*.exe|Все файлы(*.*)|*.*";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            { // Выбор файла сделан
                txtProgramm.Text = ofd.FileName;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (txtProgramm.Text.Trim().Length == 0)
            {
                MessageBox.Show("Не указано имя программы!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSelect.Focus();
                return;
            }
            try
            {
                string EventName = "Child_" + rnd.Next(1000, 100_001).ToString();
                // Preparing for synchronization between processes
                EventWaitHandle onExit = new EventWaitHandle(false, EventResetMode.ManualReset, EventName);

                // Запускаем поток для ожидания завершения дочернего процесса
                // 1 Variant, doing the same like 2 Variant, but we are using prepared Threads in our OS
                ThreadPool.QueueUserWorkItem(ThreadProcForExit, onExit);
                // 2 Variant, here we are using created Thread
                //Thread th = new Thread(ThreadProcForExit);
                //th.IsBackground = true;
                //th.Start(onExit);

                Process child = Process.Start(txtProgramm.Text, txtArgs.Text + " " + EventName); // Последний аргумент - имя глобального события.
                //Process child = Process.Start(txtProgramm.Text, generateChildName(txtArgs.Text));
                if (child != null && chIsWait.Checked)
                { // Ожидания завершения программы
                    child.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ThreadProcForExit(object param)
        {
            EventWaitHandle evn = param as EventWaitHandle;
            evn.WaitOne();
            MessageBox.Show("Дочерний процесс завершился!", "Сигнал", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}