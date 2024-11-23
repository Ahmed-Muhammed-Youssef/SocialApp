import { User } from "./User";

export class UserParams{
    minAge :  number = 18;
    maxAge : null | number = null;
    pageNumber : number = 1;
    itemsPerPage: number = 16;
    orderBy: string = 'lastActive'; // options lastActive, age, creationTime
    sex: string;
    constructor( user: User){
        this.sex = user.interest;
    }
}