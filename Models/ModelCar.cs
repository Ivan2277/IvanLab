using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarsLab
{
    public partial class ModelCar
    {
        public ModelCar()
        {
            ModelCarYear = new HashSet<ModelCarYear>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Потрібно заповнити поле")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Довжина значення від 2 до 50 символів")]
        [Display(Name = "Назва моделі авто")]
        public string ModelName { get; set; }
        public int IdEngine { get; set; }
        public int IdBody { get; set; }
        public int IdPrice { get; set; }
        [Display(Name = "Тип кузова авто")]
        public virtual BodyType IdBodyNavigation { get; set; }
        [Display(Name = "Об'єм двигуна")]
        public virtual Engine IdEngineNavigation { get; set; }
        [Display(Name = "Цінова категорія")]
        public virtual PriceCategory IdPriceNavigation { get; set; }
        public virtual ICollection<ModelCarYear> ModelCarYear { get; set; }
    }
}
