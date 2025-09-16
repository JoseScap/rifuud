import { useLocation } from 'react-router-dom';
import {
  Breadcrumb,
  BreadcrumbItem,
  BreadcrumbLink,
  BreadcrumbList,
  BreadcrumbPage,
  BreadcrumbSeparator,
} from "@/components/ui/breadcrumb";

interface BreadcrumbItem {
  label: string;
  href?: string;
}

const routeMap: Record<string, BreadcrumbItem[]> = {
  '/dashboard': [
    { label: 'Dashboard' }
  ],
  '/restaurants': [
    { label: 'Dashboard', href: '/dashboard' },
    { label: 'Restaurants' }
  ]
};

// Function to generate breadcrumb for dynamic routes
const generateBreadcrumbForRoute = (pathname: string): BreadcrumbItem[] => {
  // Handle restaurant details route
  if (pathname.startsWith('/restaurants/') && pathname !== '/restaurants') {
    return [
      { label: 'Dashboard', href: '/dashboard' },
      { label: 'Restaurants', href: '/restaurants' },
      { label: 'Restaurant Details' }
    ];
  }
  
  return [];
};

export function DynamicBreadcrumb() {
  const location = useLocation();
  
  // First check for exact matches in routeMap
  let breadcrumbItems = routeMap[location.pathname];
  
  // If no exact match, try to generate breadcrumb for dynamic routes
  if (!breadcrumbItems) {
    breadcrumbItems = generateBreadcrumbForRoute(location.pathname);
  }
  
  // Fallback if still no breadcrumb found
  if (!breadcrumbItems || breadcrumbItems.length === 0) {
    breadcrumbItems = [
      { label: 'Home', href: '/dashboard' },
      { label: 'Page' }
    ];
  }

  return (
    <Breadcrumb>
      <BreadcrumbList>
        {breadcrumbItems.map((item, index) => (
          <div key={index} className="flex items-center">
            <BreadcrumbItem>
              {index === breadcrumbItems.length - 1 ? (
                <BreadcrumbPage>{item.label}</BreadcrumbPage>
              ) : (
                <BreadcrumbLink href={item.href}>
                  {item.label}
                </BreadcrumbLink>
              )}
            </BreadcrumbItem>
            {index < breadcrumbItems.length - 1 && <BreadcrumbSeparator />}
          </div>
        ))}
      </BreadcrumbList>
    </Breadcrumb>
  );
}
