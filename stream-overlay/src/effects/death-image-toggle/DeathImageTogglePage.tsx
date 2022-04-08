import { ChangeEvent, useState } from 'react';
import styled from 'styled-components';

import defaultAliveImage from './alive.png';
import defaultDeadImage from './dead.png';

import { useLogContext } from '../../log/LogContext';
import '../common/effects-common.css';

const imageMimeType = /image\/(png|jpg|jpeg|gif)/i;

const Button = styled.button`
  margin: 0px 0.5rem;
`;

interface DeathImageTogglePageProps {
  isAlive: boolean,
}

class StateImage {
  fallback: string
  selected?: string;

  constructor(fallback: string) {
    this.fallback = fallback;
  }

  get(): string {
    return this.selected ?? this.fallback;
  }
}

function DeathImageTogglePage(props: DeathImageTogglePageProps) {
  const log = useLogContext();

  const [isAlive, setAliveState] = useState<boolean>(props.isAlive);
  const [aliveImage, setAliveImage] = useState<StateImage>(new StateImage(defaultAliveImage));
  const [deadImage, setDeadImage] = useState<StateImage>(new StateImage(defaultDeadImage));

  function handleAliveImageSelection(e: ChangeEvent<HTMLInputElement>) {
    handleImageSelection(e);
  }

  function handleDeadImageSelection(e: ChangeEvent<HTMLInputElement>) { 
    handleImageSelection(e);
  }

  function handleImageSelection(e: ChangeEvent<HTMLInputElement>) {
    if (e.target.files === null) {
      return;
    }
    const file = e.target.files[0];

    if (!file) {
      log.error('File could not be selected.');
      return;
    }
    if (!file.type.match(imageMimeType)) {
      log.error('Image mime type is not valid.');
    }

    log.trace(`Picked file of type ${file.type}`);
  }

  function setAlive() {
    setAliveState(true);
    log.trace('Test alive state');
  }

  function setDead() {
    setAliveState(false);
    log.trace('Test dead state');
  }

  return (
    <div>
      <h2>Death image toggle</h2>
      <img src={isAlive ? aliveImage.get() : deadImage.get()} />

      <div className='effect-config'>
        <div className='effect-config-section'>
          <div className='image-selection'>
            <label htmlFor='file'>Select alive image</label>
            <input type='file' accept='image/*' onChange={handleAliveImageSelection} />
          </div>
          <div className='image-selection'>
            <label htmlFor='file'>Select dead image</label>
            <input type='file' accept='image/*' onChange={handleDeadImageSelection} />
          </div>
        </div>

        <div className='effect-config-section'>
          <Button onClick={setAlive}>Test Alive</Button>
          <Button onClick={setDead}>Test Dead</Button>
        </div>
      </div>
    </div>
  );
}

DeathImageTogglePage.path = '/death-image-toggle';

export default DeathImageTogglePage;