using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DTOs
{
    public record OrderDetaialsDTO(
        [Required] int OrderId,
        [Required] int MenuId,
        [Required] int ClientId,
        [Required] string Name,
        [Required, EmailAddress] string Email,
        [Required] string Address,
        [Required] string PhoneNumber,
        [Required] string MenuName,
        [Required] int PurchaseQuantity,
        [Required, DataType(DataType.Currency)] decimal UnitPrice,
        [Required, DataType(DataType.Currency)] decimal TotalPrice,
        [Required] DateTime OrderedDate
        );

}
