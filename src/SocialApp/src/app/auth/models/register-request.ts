import { Gender } from "./user-dto";

export interface RegisterRequst{
    firstName: string;
    lastName: string;
    gender : Gender;
    email: string;
    password: string;
    dateOfBirth: string;
    cityId: number;
}