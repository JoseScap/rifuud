import { ProtectedPageLayout } from "@/layouts/protected-page-layout";
import { useRestaurant } from "./hooks/use-restaurant";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Button } from "@/components/ui/button";
import { Tooltip, TooltipContent, TooltipProvider, TooltipTrigger } from "@/components/ui/tooltip";
import { Eye, Plus } from "lucide-react";
import { useNavigate } from "react-router-dom";

export function RestaurantsPage() {
  const { restaurants, loading, error } = useRestaurant();
  const navigate = useNavigate();

  if (loading) {
    return (
      <ProtectedPageLayout>
        <div className="p-6">
          <h1 className="text-2xl font-bold mb-4">Restaurants</h1>
          <p className="text-muted-foreground">Loading restaurants...</p>
        </div>
      </ProtectedPageLayout>
    );
  }

  if (error) {
    return (
      <ProtectedPageLayout>
        <div className="p-6">
          <h1 className="text-2xl font-bold mb-4">Restaurants</h1>
          <p className="text-red-500">Error: {error}</p>
        </div>
      </ProtectedPageLayout>
    );
  }

  return (
    <ProtectedPageLayout>
      <div className="p-6">
        <div className="flex items-center justify-between mb-6">
          <div>
            <h1 className="text-2xl font-bold">Restaurants</h1>
            <p className="text-muted-foreground">
              Manage your restaurants here. Found {restaurants.length} restaurants.
            </p>
          </div>
          <Button onClick={() => navigate("/restaurants/create")}>
            <Plus className="h-4 w-4 mr-2" />
            Create Restaurant
          </Button>
        </div>
        
        <div className="rounded-md border">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Name</TableHead>
                <TableHead>Subdomain</TableHead>
                <TableHead>Status</TableHead>
                <TableHead>Created At</TableHead>
                <TableHead>Updated At</TableHead>
                <TableHead>Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {restaurants.length === 0 ? (
                <TableRow>
                  <TableCell colSpan={6} className="text-center text-muted-foreground">
                    No restaurants found
                  </TableCell>
                </TableRow>
              ) : (
                restaurants.map((restaurant) => (
                  <TableRow key={restaurant.id}>
                    <TableCell className="font-medium">{restaurant.name}</TableCell>
                    <TableCell>{restaurant.subdomain}</TableCell>
                    <TableCell>
                      <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${
                        restaurant.isActive 
                          ? 'bg-green-100 text-green-800' 
                          : 'bg-red-100 text-red-800'
                      }`}>
                        {restaurant.isActive ? 'Active' : 'Inactive'}
                      </span>
                    </TableCell>
                    <TableCell>{new Date(restaurant.createdAt).toLocaleDateString()}</TableCell>
                    <TableCell>{new Date(restaurant.updatedAt).toLocaleDateString()}</TableCell>
                    <TableCell>
                      <TooltipProvider>
                        <Tooltip>
                          <TooltipTrigger asChild>
                            <Button
                              variant="ghost"
                              size="sm"
                              onClick={() => navigate(`/restaurants/${restaurant.id}`)}
                            >
                              <Eye className="h-4 w-4" />
                            </Button>
                          </TooltipTrigger>
                          <TooltipContent>
                            <p>See Details</p>
                          </TooltipContent>
                        </Tooltip>
                      </TooltipProvider>
                    </TableCell>
                  </TableRow>
                ))
              )}
            </TableBody>
          </Table>
        </div>
      </div>
    </ProtectedPageLayout>
  );
}
