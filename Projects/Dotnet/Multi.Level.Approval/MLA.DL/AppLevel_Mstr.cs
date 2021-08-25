using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MLA.DL
{
    public class AppLevel_Mstr
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string entry_ty;
        public string Entry_ty
        {
            get { return entry_ty; }
            set { entry_ty = value; }
        }

        private int applevel;
        public int Applevel
        {
            get { return applevel; }
            set { applevel = value; }
        }

        private string user_role;
        public string User_role
        {
            get { return user_role; }
            set { user_role = value; }
        }

        private string add_user;
        public string Add_user
        {
            get { return add_user; }
            set { add_user = value; }
        }

        private DateTime add_dt;
        public DateTime Add_dt
        {
            get { return add_dt; }
            set { add_dt = value; }
        }

        private string edit_user;
        public string Edit_user
        {
            get { return edit_user; }
            set { edit_user = value; }
        }

        private DateTime edit_dt;
        public DateTime Edit_dt
        {
            get { return edit_dt; }
            set { edit_dt = value; }
        }
    }
}
