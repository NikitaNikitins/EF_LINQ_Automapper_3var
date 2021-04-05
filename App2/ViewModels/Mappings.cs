using App2.ProjcetionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2.ViewModels
{
    public class Mappings : AutoMapper.Profile
    {
        public Mappings()
        {
            CreateMap<ExaminationProjectionModel, ExaminationViewModel>();

            CreateMap<BaseGradeInfoProjectionModel, BaseGradeInfoViewModel>();

            CreateMap<PerfectGradeInfoProjectionModel, PerfectGradeInfoViewModel>();
        }
    }
}
