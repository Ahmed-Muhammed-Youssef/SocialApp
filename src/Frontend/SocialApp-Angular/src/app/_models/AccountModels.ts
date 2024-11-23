import { User } from "./User";

export interface LoginResponse {
  userData: User,
  token: string
}
export interface RegisterModel {
  userName: string;
  firstName: string;
  lastName: string;
  sex: string;
  interest: string;
  email: string;
  password: string;
  dateOfBirth: Date;
  city: string;
  country: string
}

export interface LoginModel {
  email: string,
  password: string,
}