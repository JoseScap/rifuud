import { createContext, useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

type AuthContextType = {
  token: string | null;
  isAuthenticated: boolean;
  login: (token: string) => void;
  logout: () => void;
};

const AuthContext = createContext<AuthContextType | undefined>({
  token: null,
  isAuthenticated: false,
  login: () => {},
  logout: () => {},
});

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [token, setToken] = useState<string | null>(null);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const navigate = useNavigate();

  const login = (token: string) => {
    setToken(token);
    sessionStorage.setItem('authToken', token);
    setIsAuthenticated(true);
    navigate("/dashboard");
  };

  const logout = () => {
    setToken(null);
    setIsAuthenticated(false);
    navigate("/login");
  };

  useEffect(() => {
    const token = sessionStorage.getItem('authToken');
    if (token) {
      setToken(token);
      setIsAuthenticated(true);
    } else {
      setIsAuthenticated(false);
      setToken(null);
      navigate("/login");
    }
  }, []);

  return (
    <AuthContext.Provider value={{ token, isAuthenticated, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};