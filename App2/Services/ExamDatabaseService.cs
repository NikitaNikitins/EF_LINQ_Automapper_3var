using App2.ProjcetionModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2.Services
{
    public class ExamDatabaseService : BaseService<Examenation>, IExamDatabaseService
    {
        public ExamDatabaseService(ExamEntities dataContext) : base(dataContext)
        {
        }

        public IQueryable<ExaminationProjectionModel> GetExaminationInfoBySubject(string subject)
        {
            return from examenation in dataContext.Examenations
                   join student in dataContext.Students on examenation.Ref_Stud equals student.ID_Stud
                   join subject1 in dataContext.Subjects on examenation.Ref_Subj equals subject1.ID_Subj
                   where subject1.Subject1 == subject
                   select new ExaminationProjectionModel
                   {
                       TeacherName = subject1.Teacher,
                       StudentName = student.Student1,
                       StudentCard = student.St_Card,
                       Mark = (int)examenation.Mark,
                       Subject = subject1.Subject1
                   };
        }

        public IQueryable<PerfectGradeInfoProjectionModel> GetInfoAboutGradesByGreaterThanGrade(int grade)
        {
            return from examenation in dataContext.Examenations
                   join student in dataContext.Students on examenation.Ref_Stud equals student.ID_Stud
                   join subject1 in dataContext.Subjects on examenation.Ref_Subj equals subject1.ID_Subj
                   where examenation.Mark > grade
                   select new PerfectGradeInfoProjectionModel
                   {
                       StudentName = student.Student1,
                       Mark = (int)examenation.Mark,
                       Subject = subject1.Subject1
                   };
        }
    }

    public interface IExamDatabaseService
    {
        IQueryable<ExaminationProjectionModel> GetExaminationInfoBySubject(string subject);
        IQueryable<PerfectGradeInfoProjectionModel> GetInfoAboutGradesByGreaterThanGrade(int grade);
    }
}
