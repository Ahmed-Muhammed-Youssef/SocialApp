export interface LoginResponse {
  email: string,
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
  bio: string;
  city: string;
  country: string
}

export interface LoginModel {
  email: string,
  password: string,
}
