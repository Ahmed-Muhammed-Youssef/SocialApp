export interface Picture {
  id: number;
  url: string;
}

export interface User {
  id: number;
  username: string;
  firstName: string;
  lastName: string;
  profilePictureUrl: string;
  sex: string;
  interest: string;
  age: number;
  created: Date;
  lastActive: Date;
  bio: string;
  city: string;
  country: string;
  pictures: Picture[];
  roles: string[];
}

export interface UpdateUser {
  firstName: string;
  lastName: string;
  interest: string;
  bio: string;
  city: string;
  country: string;
}
