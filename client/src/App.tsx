import { BrowserRouter, Route, Routes } from "react-router-dom";
import "./App.css";
import GamePage from "./views/GamePage";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { useEffect, useState } from "react";
import ConnectionBar from "./components/ConnectionBar";

const queryClient = new QueryClient();

function App() {
  const [isconnected, setIsconnected] = useState(false);

  useEffect(() => {
    const handleEvent = (event: CustomEvent<boolean>) => {
      setIsconnected(event.detail);
    };

    window.addEventListener("connectionUpdate", handleEvent as EventListener);

    return () => {
      window.removeEventListener(
        "connectionUpdate",
        handleEvent as EventListener,
      );
    };
  }, []);

  return (
    <QueryClientProvider client={queryClient}>
      <header className="sticky top-0 flex h-12 w-full items-center bg-gray-900 text-white">
        <div className="container mx-auto flex max-w-screen-lg justify-between p-10">
          <a className="navbar-brand" href="/">
            Tic tac toe
          </a>
          <div>
            <ConnectionBar isconnected={isconnected} />
          </div>
          <nav>
            <ul className="flex gap-4">
              <li>
                <a href="/">Game</a>
              </li>
            </ul>
          </nav>
        </div>
      </header>

      <main>
        <div className="container mx-auto max-w-screen-lg p-10">
          <BrowserRouter>
            <Routes>
              <Route path="/" element={<GamePage />} />
            </Routes>
          </BrowserRouter>
        </div>
      </main>
    </QueryClientProvider>
  );
}

export default App;
