﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wchl.WMBlog.IRepository;
using Wchl.WMBlog.IServices;
using Wchl.WMBlog.Model.Models;
using Wchl.WMBlog.Services.Base;

namespace Wchl.WMBlog.Services
{
     public partial class PasswordLibServices : BaseServices<PasswordLib>, IPasswordLibServices
    {
        IPasswordLibRepository dal;

        public PasswordLibServices(IPasswordLibRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }

    }
}