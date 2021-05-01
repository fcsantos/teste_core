using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;

namespace Core.Data.Repository
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    { 
        public QuestionRepository(MyDbContext context) : base(context) { }

    }
}
