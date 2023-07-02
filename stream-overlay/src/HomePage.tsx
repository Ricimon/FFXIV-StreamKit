import styled from 'styled-components';

import DeathImageTogglePage from './effects/death-image-toggle/DeathImageTogglePage';
import PlayVideoOnDeathPage from './effects/play-video-on-death/PlayVideoOnDeathPage';
import LinkWithQuery from './router/LinkWithQuery';

const Button = styled.button`
  margin: 0px 0.5rem;
`;

function HomePage() {
  return (
    <div>
      <h1>Effects</h1>
      <div>
        <LinkWithQuery to={DeathImageTogglePage.path}>
          <Button>Death Image Toggle</Button>
        </LinkWithQuery>
        <LinkWithQuery to={PlayVideoOnDeathPage.path}>
          <Button>Play Video On Death</Button>
        </LinkWithQuery>
      </div>
    </div>
  );
}

export default HomePage;
