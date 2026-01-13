import { UserDTO } from './user-dto';

export interface AuthResponse {
  userData: UserDTO;
  token: string;
}
