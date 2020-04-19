using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarsLab
{
    public partial class Engine
    {
        public Engine()
        {
            ModelCar = new HashSet<ModelCar>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Потрібно заповнити поле")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Довжина значення від 2 до 50 символів")]
        [RegularExpression(@"^(0|[1-9]+[0-9]*)(\.[0-9]+( )?)?L$", ErrorMessage = "Введіть десятковий дріб у форматі напр. :1.25L, 0.8L або просто 10L")]
        [Display(Name = "Об'єм двигуна")]
        public string EngineCapacity { get; set; }

        public virtual ICollection<ModelCar> ModelCar { get; set; }
    }
}
