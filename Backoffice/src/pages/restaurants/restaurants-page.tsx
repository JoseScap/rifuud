import { ProtectedPageLayout } from "@/layouts/protected-page-layout";

export function RestaurantsPage() {
  return (
    <ProtectedPageLayout>
      <div className="p-6">
        <h1 className="text-2xl font-bold mb-4">Restaurants</h1>
        <p className="text-muted-foreground">
          Manage your restaurants here.
        </p>
      </div>
    </ProtectedPageLayout>
  );
}
