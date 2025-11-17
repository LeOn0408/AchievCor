import type { UserInfoDto } from "./user.info.dto";

export interface AuthResponseDto {
  user: UserInfoDto;
  jwtToken: string;
  tokenExpiryDate: string;
}
