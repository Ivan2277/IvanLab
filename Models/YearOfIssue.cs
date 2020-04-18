using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarsLab
{
    public partial class YearOfIssue
    {
        public YearOfIssue()
        {
            ModelCarYear = new HashSet<ModelCarYear>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Потрібно заповнити поле")]
        [Range(1939, 2020, ErrorMessage = "Введіть від 1939 до поточного")]
        [Display(Name = "Роки випуску моделей(можливо партій)")]
        public int Year { get; set; }

        public virtual ICollection<ModelCarYear> ModelCarYear { get; set; }
    }
}
