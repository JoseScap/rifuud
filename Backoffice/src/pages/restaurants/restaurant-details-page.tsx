import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { ProtectedPageLayout } from "@/layouts/protected-page-layout";
import { useRestaurant } from "./hooks/use-restaurant";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { ArrowLeft } from "lucide-react";
import type { Restaurant } from "@/types/Restaurant";

export function RestaurantDetailsPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { fetchRestaurantById, loading, error } = useRestaurant();
  const [restaurant, setRestaurant] = useState<Restaurant | null>(null);

  useEffect(() => {
    if (id) {
      const loadRestaurant = async () => {
        try {
          const data = await fetchRestaurantById(id);
          setRestaurant(data);
        } catch (err) {
          console.error("Failed to load restaurant:", err);
        }
      };
      loadRestaurant();
    }
  }, [id]);

  if (loading) {
    return (
      <ProtectedPageLayout>
        <div className="p-6">
          <div className="flex items-center gap-4 mb-6">
            <Button
              variant="ghost"
              size="sm"
              onClick={() => navigate("/restaurants")}
            >
              <ArrowLeft className="h-4 w-4 mr-2" />
              Back to Restaurants
            </Button>
          </div>
          <div className="text-center text-muted-foreground">
            Loading restaurant details...
          </div>
        </div>
      </ProtectedPageLayout>
    );
  }

  if (error || !restaurant) {
    return (
      <ProtectedPageLayout>
        <div className="p-6">
          <div className="flex items-center gap-4 mb-6">
            <Button
              variant="ghost"
              size="sm"
              onClick={() => navigate("/restaurants")}
            >
              <ArrowLeft className="h-4 w-4 mr-2" />
              Back to Restaurants
            </Button>
          </div>
          <div className="text-center text-red-500">
            {error || "Restaurant not found"}
          </div>
        </div>
      </ProtectedPageLayout>
    );
  }

  return (
    <ProtectedPageLayout>
      <div className="p-6">
        <div className="flex items-center gap-4 mb-6">
          <Button
            variant="ghost"
            size="sm"
            onClick={() => navigate("/restaurants")}
          >
            <ArrowLeft className="h-4 w-4 mr-2" />
            Back to Restaurants
          </Button>
        </div>

        <div className="max-w-4xl mx-auto">
          <Card>
            <CardHeader>
              <div className="flex items-center justify-between">
                <div>
                  <CardTitle className="text-2xl">{restaurant.name}</CardTitle>
                  <CardDescription>
                    Restaurant Details
                  </CardDescription>
                </div>
                <Badge variant={restaurant.isActive ? "default" : "secondary"}>
                  {restaurant.isActive ? "Active" : "Inactive"}
                </Badge>
              </div>
            </CardHeader>
            <CardContent className="space-y-6">
              <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                <div>
                  <h3 className="font-semibold text-sm text-muted-foreground mb-2">
                    SUBDOMAIN
                  </h3>
                  <p className="text-lg">{restaurant.subdomain}</p>
                </div>
                
                <div>
                  <h3 className="font-semibold text-sm text-muted-foreground mb-2">
                    RESTAURANT ID
                  </h3>
                  <p className="text-lg font-mono text-sm">{restaurant.id}</p>
                </div>
                
                <div>
                  <h3 className="font-semibold text-sm text-muted-foreground mb-2">
                    CREATED AT
                  </h3>
                  <p className="text-lg">
                    {new Date(restaurant.createdAt).toLocaleString()}
                  </p>
                </div>
                
                <div>
                  <h3 className="font-semibold text-sm text-muted-foreground mb-2">
                    UPDATED AT
                  </h3>
                  <p className="text-lg">
                    {new Date(restaurant.updatedAt).toLocaleString()}
                  </p>
                </div>
              </div>
            </CardContent>
          </Card>
        </div>
      </div>
    </ProtectedPageLayout>
  );
}
