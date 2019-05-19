using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clube_de_Membros.Models
{
    public class MembersViewModel
    {
        public LinkedList<Members> filteredMembers;
        public int currentPage, maxPages;
    }
}