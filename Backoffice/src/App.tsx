import { BrowserRouter, Route, Routes } from 'react-router-dom'
import './App.css'
import { AuthProvider } from './providers/AuthProvider'
import { HomePage } from './pages/home/home-page'
import { LoginPage } from './pages/login/login-page'
import { DashboardPage } from './pages/dashboard/dashboard-page'
import { RestaurantsPage } from './pages/restaurants/restaurants-page'
import { RestaurantDetailsPage } from './pages/restaurants/restaurant-details-page'
import { RestaurantCreatePage } from './pages/restaurants/restaurant-create-page'

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
            <Route path="/restaurants/create" element={<RestaurantCreatePage />} />
            <Route path="/restaurants/:id" element={<RestaurantDetailsPage />} />
          </Routes>
        </AuthProvider>
      </BrowserRouter>
    </>
  )
}

export default App
