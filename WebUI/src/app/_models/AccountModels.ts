export interface LoginResponse {
  email: string,
  token: string
}
export interface RegisterModel {
  firstName: string,
  lastName: string,
  sex: string,
  interest: string,
  email: string,
  password: string
}

export interface LoginModel {
  email: string,
  password: string,
}
