using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementBLL.ViewModels.PlanViewModels
{
    public class UpdatePlanViewModel
    {
        public string PlanName { get; set; } = null!;

        [Required(ErrorMessage = "Price Name Is Required")]
        [Range(0.1,10000, ErrorMessage = "Price Must Be Between 0.1 and 10000")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Description Name Is Required")]
        [StringLength(200,MinimumLength =5, ErrorMessage = "Description Can't Be More Than 200 Or Less Than 5 Characters")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Duration Days Is Required")]
        [Range(1, 365, ErrorMessage = "Duration Days Must Be Between 1 and 365")]
        public int DurationDays { get; set; }
    }
}
