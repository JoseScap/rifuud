import { useEffect, useState } from "react";
import apiClient from "@/lib/apiClient";
import type { Restaurant, RestaurantListManyResponse } from "@/types/Restaurant";

export function useRestaurant() {
  const [restaurants, setRestaurants] = useState<Restaurant[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchRestaurants = async () => {
      try {
        setLoading(true);
        setError(null);
        const response = await apiClient.get<RestaurantListManyResponse>("/Backoffice/Restaurant");
        console.log(response.data);
        setRestaurants(response.data.restaurants);
      } catch (err) {
        setError(err instanceof Error ? err.message : "Failed to fetch restaurants");
        console.error("Error fetching restaurants:", err);
      } finally {
        setLoading(false);
      }
    };
    
    fetchRestaurants();
  }, []);

  const fetchRestaurantById = async (id: string) => {
    try {
      setLoading(true);
      setError(null);
      const response = await apiClient.get<Restaurant>(`/Backoffice/Restaurant/${id}`);
      return response.data;
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to fetch restaurant");
      console.error("Error fetching restaurant:", err);
      throw err;
    } finally {
      setLoading(false);
    }
  };

  const createRestaurant = async (restaurantData: { name: string; subdomain: string; isActive: boolean }) => {
    try {
      setLoading(true);
      setError(null);
      const response = await apiClient.post<Restaurant>("/Backoffice/Restaurant", restaurantData);
      return response.data;
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to create restaurant");
      console.error("Error creating restaurant:", err);
      throw err;
    } finally {
      setLoading(false);
    }
  };

  return {
    restaurants,
    loading,
    error,
    refetch: () => {
      setLoading(true);
      setError(null);
      return apiClient.get<RestaurantListManyResponse>("/Backoffice/Restaurant")
        .then(response => {
          setRestaurants(response.data.restaurants);
          return response.data;
        })
        .catch(err => {
          setError(err instanceof Error ? err.message : "Failed to fetch restaurants");
          throw err;
        })
        .finally(() => {
          setLoading(false);
        });
    },
    fetchRestaurantById,
    createRestaurant
  };
}
