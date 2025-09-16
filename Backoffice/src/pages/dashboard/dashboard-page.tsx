import { ProtectedPageLayout } from "@/layouts/protected-page-layout";

export function DashboardPage() {
  return (
    <ProtectedPageLayout>
      <div>Dashboard</div>
    </ProtectedPageLayout>
  );
}