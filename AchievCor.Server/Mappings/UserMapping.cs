using AchievCor.Server.Data.Entities;
using AchievCor.Server.Dto;

namespace AchievCor.Server.Mappings;

public static class UserMapping
{
    public static UserDto ToDto(this UserEntity userEntity)
    {
        return new UserDto
        {
            Id = userEntity.Id,
            Login = userEntity.LocalIdentity?.Login ?? userEntity.Email ?? userEntity.Id.ToString(),
            Email = userEntity.Email ?? string.Empty,
            FirstName = userEntity.FirstName,
        };
    }

    public static UserEntity ToEntity(this UserDto userDto)
    {
        return new UserEntity
        {
            Id = userDto.Id,
            FirstName = userDto.FirstName,
            Email = userDto.Email,
        };
    }
}
