using App2.Services;
using App2.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace App2
{
    public partial class Form1 : Form
    {
        private ExamDatabaseService examDatabaseService;
        private ExamEntities examEntities;
        private Mapper mapper;

        public Form1()
        {
            InitializeComponent();
            examEntities = new ExamEntities();
            examDatabaseService = new ExamDatabaseService(examEntities);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<Mappings>();
            });

            mapper = new Mapper(config);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox1.AppendText("Eksāmens C# valodā:\n");

            var examinationsInfos = mapper.Map<List<ExaminationViewModel>>(examDatabaseService.GetExaminationInfoBySubject("C#"));

            richTextBox1.AppendText("Student \t St_Card \t Subject \t Mark \t Teacher\n");
            foreach (var info in examinationsInfos)
                richTextBox1.AppendText($"{info.StudentName} \t {info.StudentCard} \t {info.Subject} \t {info.Mark} \t {info.TeacherName}\n");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox1.AppendText("Info par teicamām sekmēm:\n");

            var gradeInfos = mapper.Map<List<PerfectGradeInfoViewModel>>(examDatabaseService.GetInfoAboutGradesByGreaterThanGrade(7));

            richTextBox1.AppendText("Student \t Subject \t Mark\n");
            foreach (var info in gradeInfos)
                richTextBox1.AppendText($"{info.StudentName} \t {info.Subject} \t {info.Mark}\n");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();

            examDatabaseService.Add(new Examenation() 
            {
                Ref_Stud =2,
                Ref_Subj = 3,
                Mark = 8
            });

            examDatabaseService.Add(new Examenation()
            {
                Ref_Stud = 3,
                Ref_Subj = 2,
                Mark = 4
            });

            richTextBox1.AppendText("Info par studentu sekmēm:\n");

            var gradeInfos = mapper.Map<List<PerfectGradeInfoViewModel>>(examDatabaseService.GetInfoAboutGradesByGreaterThanGrade(0));

            richTextBox1.AppendText("Student \t Subject \t Mark\n");
            foreach (var info in gradeInfos)
                richTextBox1.AppendText($"{info.StudentName} \t {info.Subject} \t {info.Mark}\n");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();

            XElement doc = new XElement("Exams");
            var gradeInfos = mapper.Map<List<BaseGradeInfoViewModel>>(examDatabaseService.GetInfoAboutGradesByGreaterThanGrade(0));

            foreach (var item in gradeInfos)
                doc.Add(new XElement("exam",
                    new XAttribute("Student", item.StudentName),
                    new XAttribute("Subject", item.Subject),
                    new XAttribute("Mark", item.Mark)
                    ));

            doc.Save("Exam.xml");
            richTextBox1.Text = File.ReadAllText("Exam.xml");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox1.AppendText("Student   \t Subject \t Mark\n");

            XDocument doc = XDocument.Load("Exam.xml");
            var grades = from gradeInfos in doc.Descendants("exam") 
                         select new BaseGradeInfoViewModel() 
                         {
                             Mark = int.Parse(gradeInfos.Attribute("Mark").Value),
                             Subject = gradeInfos.Attribute("Subject").Value,
                             StudentName = gradeInfos.Attribute("Student").Value
                         };

            var averageMark = grades.Average(gr => gr.Mark);

            foreach (var item in grades)
                richTextBox1.AppendText($"{item.StudentName}   \t {item.Subject} \t {item.Mark}\n");

            richTextBox1.AppendText($"Atzīmju vidējā aritmētiskā vērtība: {averageMark:F4}");

            var groupedBySubject = grades.GroupBy(
                p => p.Subject,
                p => p.Mark,
                (key, g) => new
                {
                    Subject = key,
                    Average = g.Average(mark => mark)
                });

            richTextBox1.AppendText("\n\nGrouped Subjects by mark\n");

            foreach (var item in groupedBySubject)
                richTextBox1.AppendText($"{item.Subject}   \t {item.Average:f4}\n");
        }
    }
}
