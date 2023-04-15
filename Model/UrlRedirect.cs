using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace JustShortIt.Model; 

[BindProperties]
public record UrlRedirect([Required, MinLength(2), MaxLength(16)]string Id, [Required, Url]string Target, [Required]string ExpirationDate);