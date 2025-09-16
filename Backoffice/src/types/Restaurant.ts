export type Restaurant = {
  id: string;
  createdAt: string;
  updatedAt: string;
  name: string;
  subdomain: string;
  isActive: boolean;
}

export type RestaurantListManyResponse = {
  restaurants: Restaurant[];
}