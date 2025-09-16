import { LoginForm } from "@/components/login-form";
import { useAuth } from "@/providers/AuthProvider";
import type { LoginResponse } from "@/types/Auth";
import apiClient from "@/lib/apiClient";

export function LoginPage() {
  const { login } = useAuth();

  const handleSubmit = async (username: string, password: string) => {
    try {
      const response = await apiClient.post<LoginResponse>("/Auth/Admin/Login", {
        username,
        password,
      });
      
      login(response.data.accessToken);
    } catch (error) {
      console.error("Login failed:", error);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 py-12 px-4 sm:px-6 lg:px-8">
      <div className="max-w-md w-full space-y-8">
        <LoginForm onSubmit={handleSubmit} />
      </div>
    </div>
  );
}
