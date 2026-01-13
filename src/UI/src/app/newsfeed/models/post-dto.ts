export interface PostDTO {
  id: number;
  content: string;
  datePosted: string;
  dateEdited?: string | null;
  ownerId: number;
  ownerName: string;
  ownerPictureUrl: string;
}