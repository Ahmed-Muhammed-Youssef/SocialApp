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
