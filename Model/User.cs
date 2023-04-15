using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace JustShortIt.Model; 

[BindProperties]
public record User([Required]string Username, [Required]string Password);