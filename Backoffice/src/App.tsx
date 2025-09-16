import { BrowserRouter, Route, Routes } from 'react-router-dom'
import './App.css'
import { AuthProvider } from './providers/AuthProvider'
import { HomePage } from './pages/home/home-page'
import { LoginPage } from './pages/login/login-page'
import { DashboardPage } from './pages/dashboard/dashboard-page'
import { RestaurantsPage } from './pages/restaurants/restaurants-page'

function App() {
  return (
    <>
      <BrowserRouter>
        <AuthProvider>
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/dashboard" element={<DashboardPage />} />
            <Route path="/restaurants" element={<RestaurantsPage />} />
          </Routes>
        </AuthProvider>
      </BrowserRouter>
    </>
  )
}

export default App
