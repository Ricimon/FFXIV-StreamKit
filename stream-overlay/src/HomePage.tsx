import styled from 'styled-components';
import { Link } from 'react-router-dom';

import DeathImageTogglePage from './effects/death-image-toggle/DeathImageTogglePage';
import PlayVideoOnDeathPage from './effects/play-video-on-death/PlayVideoOnDeathPage';

const Button = styled.button`
  margin: 0px 0.5rem;
`;

function HomePage() {
  return (
    <div>
      <h1>Effects</h1>
      <div>
        <Link to={DeathImageTogglePage.path}>
          <Button>Death Image Toggle</Button>
        </Link>
        <Link to={PlayVideoOnDeathPage.path}>
          <Button>Play Video On Death</Button>
        </Link>
      </div>
    </div>
  );
}

export default HomePage;
