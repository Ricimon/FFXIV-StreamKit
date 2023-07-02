import { Route, Routes, useSearchParams } from 'react-router-dom';
import { Provider } from 'inversify-react';

import createContainer from './inversify.config';
import DeathImageTogglePage from './effects/death-image-toggle/DeathImageTogglePage';
import PlayVideoOnDeathPage from './effects/play-video-on-death/PlayVideoOnDeathPage';
import WebsocketStatus from './effects/common/WebsocketStatus';
import HomePage from './HomePage';
import PageNotFound from './PageNotFound';

import './App.css';
import WebsocketClientComponent from './websocket/WebsocketClientComponent';

function App() {
  let [searchParams] = useSearchParams();
  const container = createContainer(searchParams);

  return (
    <Provider container={container} key={container.id}>
      <div className='App'>
        <WebsocketClientComponent />

        <WebsocketStatus />
        <Routes>
          <Route path='/' element={<HomePage />} />
          <Route path={DeathImageTogglePage.path} element={<DeathImageTogglePage />} />
          <Route path={PlayVideoOnDeathPage.path} element={<PlayVideoOnDeathPage />} />
          <Route path='*' element={<PageNotFound />} />
        </Routes>
      </div >
    </Provider>
  );
}

export default App;
