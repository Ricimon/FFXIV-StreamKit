import { BrowserRouter, Routes, Route } from 'react-router-dom';

import LogProvider from './log/LogContext';
import ConsoleLogger from './log/ConsoleLogger';
import HomePage from './HomePage';
import DeathImageTogglePage from './effects/death-image-toggle/DeathImageTogglePage';
import PlayVideoOnDeathPage from './effects/play-video-on-death/PlayVideoOnDeathPage';
import PageNotFound from './PageNotFound';

import './App.css';

function App() {
  return (
    <div className='App'>
      <LogProvider>
        <BrowserRouter>
          <Routes>
            <Route path='/' element={<HomePage />} />
            <Route path={DeathImageTogglePage.path} element={<DeathImageTogglePage isAlive={true} />} />
            <Route path={PlayVideoOnDeathPage.path} element={<PlayVideoOnDeathPage />} />
            <Route path='*' element={<PageNotFound />} />
          </Routes>
        </BrowserRouter>

        <ConsoleLogger />
      </LogProvider>
    </div >
  );
}

export default App;
