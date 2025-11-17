import { UserDTO } from './UserDTO';

export interface AuthResponse {
  userData: UserDTO;
  token: string;
}
