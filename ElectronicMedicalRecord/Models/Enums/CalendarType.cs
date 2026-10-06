using System.ComponentModel.DataAnnotations;

namespace ElectronicMedicalRecord.Models.Enums;

public enum CalendarType
{
    [Display(Name = "Week")]
    Week,
    [Display(Name = "Day")]
    Day
}