using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarsLab
{
    public partial class BodyType
    {
        public BodyType()
        {
            ModelCar = new HashSet<ModelCar>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Потрібно заповнити поле")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Довжина значення від 2 до 50 символів")]
        [Display(Name = "Тип кузова авто")]
        public string TypeName { get; set; }

        public virtual ICollection<ModelCar> ModelCar { get; set; }
    }
}
