import { createContext, useContext, useEffect, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";

type AuthContextType = {
  token: string | null;
  isAuthenticated: boolean;
  login: (token: string) => void;
  logout: () => void;
  checkAuth: () => void;
};

const AuthContext = createContext<AuthContextType | undefined>({
  token: null,
  isAuthenticated: false,
  login: () => {},
  logout: () => {},
  checkAuth: () => {},
});

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [token, setToken] = useState<string | null>(null);
  const isAuthenticated = useMemo(() => {
    return token !== null;
  }, [token]);
  const navigate = useNavigate();

  const login = (token: string) => {
    setToken(token);
    sessionStorage.setItem('authToken', token);
    navigate("/dashboard");
  };

  const logout = () => {
    setToken(null);
    navigate("/login");
  };

  const checkAuth = () => {
    const token = sessionStorage.getItem('authToken');
    if (token) {
      setToken(token);
    } else {
      setToken(null);
      navigate("/login");
    }
  }

  useEffect(() => {
    checkAuth();
  }, [token]);

  return (
    <AuthContext.Provider value={{
      token,
      isAuthenticated,
      login,
      logout,
      checkAuth,
    }}>
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