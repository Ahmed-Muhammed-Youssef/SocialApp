export interface Photo {
  id: number;
  url: string;
  order: number;
}

export interface User {
  username: string;
  firstName: string;
  lastName: string;
  sex: string;
  interest: string;
  age: number;
  created: Date;
  lastActive: Date;
  bio: string;
  city: string;
  country: string;
  photos: Photo[];
}

export interface UpdateUser {
  firstName: string;
  lastName: string;
  interest: string;
  bio: string;
  city: string;
  country: string;
}
