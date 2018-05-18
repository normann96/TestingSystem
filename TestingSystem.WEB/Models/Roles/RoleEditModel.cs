using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestingSystem.BLL.EntitiesDto;

namespace TestingSystem.WEB.Models.Roles
{
    public class RoleEditModel
    {
        public RoleDto Role { get; set; }
        public IEnumerable<UserDto> Members { get; set; }
        public IEnumerable<UserDto> NonMembers { get; set; }
    }
}