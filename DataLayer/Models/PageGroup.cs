using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
  public  class PageGroup
    {
        [Key]
        public int GroupID { get; set; }
        [Display(Name = "عنوان گروه")]
        [MaxLength(150)]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string GroupTitle { get; set; }

        //Navigation property
        public virtual List<Page> Pages { get; set; }
        public PageGroup()
        {

        }
    }
}
