import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { ProtectedPageLayout } from "@/layouts/protected-page-layout";
import { useRestaurant } from "./hooks/use-restaurant";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Checkbox } from "@/components/ui/checkbox";
import { Alert, AlertDescription } from "@/components/ui/alert";
import { ArrowLeft, CheckCircle, AlertCircle } from "lucide-react";

export function RestaurantCreatePage() {
  const navigate = useNavigate();
  const { createRestaurant, loading, error } = useRestaurant();
  
  const [formData, setFormData] = useState({
    name: "",
    subdomain: "",
    isActive: true
  });
  
  const [success, setSuccess] = useState(false);
  const [formError, setFormError] = useState<string | null>(null);

  const handleInputChange = (field: string, value: string | boolean) => {
    setFormData(prev => ({
      ...prev,
      [field]: value
    }));
    // Clear errors when user starts typing
    if (formError) setFormError(null);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    // Basic validation
    if (!formData.name.trim()) {
      setFormError("Restaurant name is required");
      return;
    }
    
    if (!formData.subdomain.trim()) {
      setFormError("Subdomain is required");
      return;
    }

    try {
      setFormError(null);
      const newRestaurant = await createRestaurant(formData);
      setSuccess(true);
      
      // Redirect to the new restaurant details page after a short delay
      setTimeout(() => {
        navigate(`/restaurants/${newRestaurant.id}`);
      }, 2000);
      
    } catch (err) {
      console.error("Failed to create restaurant:", err);
      // Error is already handled by the hook
    }
  };

  if (success) {
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

          <div className="max-w-2xl mx-auto">
            <Alert>
              <CheckCircle className="h-4 w-4" />
              <AlertDescription>
                Restaurant created successfully! Redirecting to restaurant details...
              </AlertDescription>
            </Alert>
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

        <div className="max-w-2xl mx-auto">
          <Card>
            <CardHeader>
              <CardTitle>Create New Restaurant</CardTitle>
              <CardDescription>
                Fill in the details below to create a new restaurant.
              </CardDescription>
            </CardHeader>
            <CardContent>
              <form onSubmit={handleSubmit} className="space-y-6">
                {/* Error Alert */}
                {(error || formError) && (
                  <Alert variant="destructive">
                    <AlertCircle className="h-4 w-4" />
                    <AlertDescription>
                      {formError || error}
                    </AlertDescription>
                  </Alert>
                )}

                {/* Restaurant Name */}
                <div className="space-y-2">
                  <Label htmlFor="name">Restaurant Name *</Label>
                  <Input
                    id="name"
                    type="text"
                    placeholder="Enter restaurant name"
                    value={formData.name}
                    onChange={(e) => handleInputChange("name", e.target.value)}
                    disabled={loading}
                    required
                  />
                </div>

                {/* Subdomain */}
                <div className="space-y-2">
                  <Label htmlFor="subdomain">Subdomain *</Label>
                  <Input
                    id="subdomain"
                    type="text"
                    placeholder="Enter subdomain (e.g., myrestaurant)"
                    value={formData.subdomain}
                    onChange={(e) => handleInputChange("subdomain", e.target.value)}
                    disabled={loading}
                    required
                  />
                  <p className="text-sm text-muted-foreground">
                    This will be used for the restaurant's URL: {formData.subdomain || "subdomain"}.yourdomain.com
                  </p>
                </div>

                {/* Active Status */}
                <div className="flex items-center space-x-2">
                  <Checkbox
                    id="isActive"
                    checked={formData.isActive}
                    onCheckedChange={(checked) => handleInputChange("isActive", checked as boolean)}
                    disabled={loading}
                  />
                  <Label htmlFor="isActive">Restaurant is active</Label>
                </div>

                {/* Submit Button */}
                <div className="flex gap-4">
                  <Button
                    type="submit"
                    disabled={loading}
                    className="flex-1"
                  >
                    {loading ? "Creating..." : "Create Restaurant"}
                  </Button>
                  <Button
                    type="button"
                    variant="outline"
                    onClick={() => navigate("/restaurants")}
                    disabled={loading}
                  >
                    Cancel
                  </Button>
                </div>
              </form>
            </CardContent>
          </Card>
        </div>
      </div>
    </ProtectedPageLayout>
  );
}
