using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarsLab
{
    public partial class PriceCategory
    {
        public PriceCategory()
        {
            ModelCar = new HashSet<ModelCar>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Потрібно заповнити поле")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Довжина значення від 2 до 50 символів")]
        [RegularExpression(@"^[1-9]+[0-9]*(\-[1-9]+[0-9]*)*\$$", ErrorMessage = "Введіть проміжок у форматі напр. :200-300$, 100000-200000$ або просто 100000$")]
        [Remote("CheckRange", "PriceCategories", ErrorMessage = "Введіть ціновий проміжок у якому 1 число менше за 2")]
        [Display(Name = "Цінова категорія")]
        
        public string Price { get; set; }

        public virtual ICollection<ModelCar> ModelCar { get; set; }
    }
}
